import { HttpErrorResponse } from '@angular/common/http';

export function getApiErrorMessage(error: unknown, fallback: string): string {
  if (!(error instanceof HttpErrorResponse)) {
    return fallback;
  }

  const body = error.error;

  if (typeof body === 'object' && body !== null && 'message' in body) {
    const message = String((body as { message: unknown }).message).trim();
    return message || fallback;
  }

  if (typeof body === 'string' && body.trim()) {
    return body.trim();
  }

  return fallback;
}
