import { DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { Provider } from '../../core/models/api.models';

type FormMode = 'hidden' | 'create' | 'edit';

@Component({
  selector: 'app-providers-page',
  imports: [
    DatePipe,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatChipsModule,
    MatIconModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
  ],
  templateUrl: './providers-page.component.html',
  styleUrl: './providers-page.component.scss',
})
export class ProvidersPageComponent implements OnInit {
  private readonly api = inject(ApiService);
  private readonly notifications = inject(NotificationService);
  private readonly formBuilder = inject(FormBuilder);

  protected readonly displayedColumns = [
    'nit',
    'name',
    'website',
    'email',
    'status',
    'createdAt',
    'updatedAt',
    'actions',
  ];
  protected readonly providers = signal<Provider[]>([]);
  protected readonly totalCount = signal(0);
  protected readonly pageIndex = signal(0);
  protected readonly pageSize = signal(10);
  protected readonly includeInactive = signal(false);
  protected readonly isInitialLoading = signal(true);
  protected readonly isRefreshing = signal(false);
  protected readonly updatingId = signal<string | null>(null);
  protected readonly isSaving = signal(false);
  protected readonly errorMessage = signal('');
  protected readonly formMode = signal<FormMode>('hidden');
  protected readonly editingId = signal<string | null>(null);

  protected readonly searchControl = this.formBuilder.nonNullable.control('');
  protected readonly form = this.formBuilder.nonNullable.group({
    nit: ['', Validators.required],
    name: ['', Validators.required],
    website: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
  });

  ngOnInit(): void {
    this.loadProviders();

    this.searchControl.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe(() => {
        this.pageIndex.set(0);
        this.loadProviders();
      });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
    this.loadProviders();
  }

  onIncludeInactiveChange(checked: boolean): void {
    this.includeInactive.set(checked);
    this.pageIndex.set(0);
    this.loadProviders();
  }

  openCreateForm(): void {
    this.formMode.set('create');
    this.editingId.set(null);
    this.form.reset();
  }

  openEditForm(provider: Provider): void {
    this.formMode.set('edit');
    this.editingId.set(provider.id);
    this.form.patchValue({
      nit: provider.nit,
      name: provider.name,
      website: provider.website,
      email: provider.email,
    });
  }

  cancelForm(): void {
    this.formMode.set('hidden');
    this.editingId.set(null);
    this.form.reset();
  }

  submitForm(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const request = this.form.getRawValue();
    const mode = this.formMode();
    this.isSaving.set(true);

    if (mode === 'create') {
      this.api
        .createProvider(request)
        .pipe(finalize(() => this.isSaving.set(false)))
        .subscribe({
          next: () => {
            this.cancelForm();
            this.notifications.success('Proveedor creado correctamente.');
            this.loadProviders({ silent: true });
          },
          error: () => {
            this.notifications.error('No se pudo crear el proveedor.');
          },
        });
      return;
    }

    const id = this.editingId();
    if (!id) {
      this.isSaving.set(false);
      return;
    }

    this.api
      .updateProvider(id, request)
      .pipe(finalize(() => this.isSaving.set(false)))
      .subscribe({
        next: () => {
          this.cancelForm();
          this.notifications.success('Proveedor actualizado correctamente.');
          this.loadProviders({ silent: true });
        },
        error: () => {
          this.notifications.error('No se pudo actualizar el proveedor.');
        },
      });
  }

  toggleStatus(provider: Provider): void {
    if (this.updatingId()) {
      return;
    }

    const wasActive = provider.isActive;
    this.updatingId.set(provider.id);

    const action = wasActive
      ? this.api.deactivateProvider(provider.id)
      : this.api.activateProvider(provider.id);

    action.pipe(finalize(() => this.updatingId.set(null))).subscribe({
      next: () => {
        this.notifications.success(
          wasActive ? 'Proveedor inactivado.' : 'Proveedor activado.'
        );
        this.loadProviders({ silent: true });
      },
      error: () => {
        this.notifications.error('No se pudo cambiar el estado del proveedor.');
      },
    });
  }

  isRowBusy(id: string): boolean {
    return this.updatingId() === id;
  }

  private loadProviders(options: { silent?: boolean } = {}): void {
    const hasData = this.providers().length > 0;

    if (!options.silent && !hasData) {
      this.isInitialLoading.set(true);
    } else {
      this.isRefreshing.set(true);
    }

    this.errorMessage.set('');

    this.api
      .getProviders({
        page: this.pageIndex() + 1,
        pageSize: this.pageSize(),
        search: this.searchControl.value || undefined,
        sortBy: 'name',
        sortDirection: 'asc',
        includeInactive: this.includeInactive(),
      })
      .pipe(
        finalize(() => {
          this.isInitialLoading.set(false);
          this.isRefreshing.set(false);
        })
      )
      .subscribe({
        next: (result) => {
          this.providers.set(result.items);
          this.totalCount.set(result.totalCount);
        },
        error: () => {
          this.errorMessage.set('No se pudieron cargar los proveedores.');
        },
      });
  }
}
