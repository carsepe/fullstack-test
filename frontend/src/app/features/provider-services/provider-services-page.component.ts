import { DatePipe, DecimalPipe } from '@angular/common';
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
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { NotificationService } from '../../core/services/notification.service';
import { getApiErrorMessage } from '../../core/utils/api-error.util';
import { Provider, ProviderService } from '../../core/models/api.models';

type FormMode = 'hidden' | 'create' | 'edit';

@Component({
  selector: 'app-provider-services-page',
  imports: [
    DatePipe,
    DecimalPipe,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatChipsModule,
    MatIconModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
  ],
  templateUrl: './provider-services-page.component.html',
  styleUrl: './provider-services-page.component.scss',
})
export class ProviderServicesPageComponent implements OnInit {
  private readonly api = inject(ApiService);
  private readonly notifications = inject(NotificationService);
  private readonly formBuilder = inject(FormBuilder);

  protected readonly displayedColumns = [
    'name',
    'provider',
    'hourlyRateUsd',
    'status',
    'createdAt',
    'updatedAt',
    'actions',
  ];
  protected readonly services = signal<ProviderService[]>([]);
  protected readonly providers = signal<Provider[]>([]);
  protected readonly totalCount = signal(0);
  protected readonly pageIndex = signal(0);
  protected readonly pageSize = signal(10);
  protected readonly includeInactive = signal(false);
  protected readonly filterProviderId = signal('');
  protected readonly isInitialLoading = signal(true);
  protected readonly isRefreshing = signal(false);
  protected readonly updatingId = signal<string | null>(null);
  protected readonly isSaving = signal(false);
  protected readonly errorMessage = signal('');
  protected readonly formMode = signal<FormMode>('hidden');
  protected readonly editingId = signal<string | null>(null);

  protected readonly searchControl = this.formBuilder.nonNullable.control('');
  protected readonly form = this.formBuilder.nonNullable.group({
    providerId: ['', Validators.required],
    name: ['', Validators.required],
    hourlyRateUsd: [0, [Validators.required, Validators.min(0.01)]],
  });

  ngOnInit(): void {
    this.loadProviders();
    this.loadServices();

    this.searchControl.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe(() => {
        this.pageIndex.set(0);
        this.loadServices();
      });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
    this.loadServices();
  }

  onFiltersChange(): void {
    this.pageIndex.set(0);
    this.loadServices();
  }

  onProviderFilterChange(providerId: string): void {
    this.filterProviderId.set(providerId);
    this.onFiltersChange();
  }

  onIncludeInactiveChange(checked: boolean): void {
    this.includeInactive.set(checked);
    this.onFiltersChange();
  }

  getProviderName(providerId: string): string {
    return this.providers().find((provider) => provider.id === providerId)?.name ?? providerId;
  }

  openCreateForm(): void {
    this.formMode.set('create');
    this.editingId.set(null);
    this.form.reset({ providerId: this.filterProviderId() || '', hourlyRateUsd: 0 });
  }

  openEditForm(service: ProviderService): void {
    this.formMode.set('edit');
    this.editingId.set(service.id);
    this.form.patchValue({
      providerId: service.providerId,
      name: service.name,
      hourlyRateUsd: service.hourlyRateUsd,
    });
    this.form.controls.providerId.disable();
  }

  cancelForm(): void {
    this.formMode.set('hidden');
    this.editingId.set(null);
    this.form.controls.providerId.enable();
    this.form.reset();
  }

  submitForm(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSaving.set(true);

    if (this.formMode() === 'create') {
      const request = this.form.getRawValue();
      this.api
        .createProviderService(request)
        .pipe(finalize(() => this.isSaving.set(false)))
        .subscribe({
          next: () => {
            this.cancelForm();
            this.notifications.success('Servicio creado correctamente.');
            this.loadServices({ silent: true });
          },
          error: (error) => {
            this.notifications.error(
              getApiErrorMessage(error, 'No se pudo crear el servicio.')
            );
          },
        });
      return;
    }

    const id = this.editingId();
    if (!id) {
      this.isSaving.set(false);
      return;
    }

    const { name, hourlyRateUsd } = this.form.getRawValue();
    this.api
      .updateProviderService(id, { name, hourlyRateUsd })
      .pipe(finalize(() => this.isSaving.set(false)))
      .subscribe({
        next: () => {
          this.cancelForm();
          this.notifications.success('Servicio actualizado correctamente.');
          this.loadServices({ silent: true });
        },
        error: (error) => {
          this.notifications.error(
            getApiErrorMessage(error, 'No se pudo actualizar el servicio.')
          );
        },
      });
  }

  toggleStatus(service: ProviderService): void {
    if (this.updatingId()) {
      return;
    }

    const wasActive = service.isActive;
    this.updatingId.set(service.id);

    const action = wasActive
      ? this.api.deactivateProviderService(service.id)
      : this.api.activateProviderService(service.id);

    action.pipe(finalize(() => this.updatingId.set(null))).subscribe({
      next: () => {
        this.notifications.success(
          wasActive ? 'Servicio inactivado.' : 'Servicio activado.'
        );
        this.loadServices({ silent: true });
      },
      error: (error) => {
        this.notifications.error(
          getApiErrorMessage(error, 'No se pudo cambiar el estado del servicio.')
        );
      },
    });
  }

  isRowBusy(id: string): boolean {
    return this.updatingId() === id;
  }

  private loadProviders(): void {
    this.api
      .getProviders({
        page: 1,
        pageSize: 100,
        sortBy: 'name',
        sortDirection: 'asc',
        includeInactive: true,
      })
      .subscribe({
        next: (result) => {
          this.providers.set(result.items);
        },
      });
  }

  private loadServices(options: { silent?: boolean } = {}): void {
    const hasData = this.services().length > 0;

    if (!options.silent && !hasData) {
      this.isInitialLoading.set(true);
    } else {
      this.isRefreshing.set(true);
    }

    this.errorMessage.set('');

    this.api
      .getProviderServices(
        {
          page: this.pageIndex() + 1,
          pageSize: this.pageSize(),
          search: this.searchControl.value || undefined,
          sortBy: 'name',
          sortDirection: 'asc',
          includeInactive: this.includeInactive(),
        },
        this.filterProviderId() || undefined
      )
      .pipe(
        finalize(() => {
          this.isInitialLoading.set(false);
          this.isRefreshing.set(false);
        })
      )
      .subscribe({
        next: (result) => {
          this.services.set(result.items);
          this.totalCount.set(result.totalCount);
        },
        error: () => {
          this.errorMessage.set('No se pudieron cargar los servicios.');
        },
      });
  }
}
