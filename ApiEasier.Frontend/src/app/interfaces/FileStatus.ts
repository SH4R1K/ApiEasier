import type { TuiFileLike } from '@taiga-ui/kit';

export interface FileStatus {
  file: TuiFileLike;
  status: 'loading' | 'normal' | 'error' | 'success';
  errorMessage: string;
}
