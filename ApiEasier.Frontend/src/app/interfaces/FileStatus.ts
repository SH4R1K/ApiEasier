import type { TuiFileLike } from '@taiga-ui/kit';

/**
 * Интерфейс, представляющий статус файла.
 *
 * @remarks
 * Этот интерфейс используется для описания состояния файла, включая статус загрузки и сообщение об ошибке.
 *
 * @type {{ file: TuiFileLike, status: 'loading' | 'normal' | 'error' | 'success', errorMessage: string }}
 * @default { file: {}, status: 'normal', errorMessage: '' }
 * @memberof Component
 */
export interface FileStatus {
  /**
   * Объект файла.
   * @type {TuiFileLike}
   */
  file: TuiFileLike;

  /**
   * Статус файла.
   * @type {'loading' | 'normal' | 'error' | 'success'}
   */
  status: 'loading' | 'normal' | 'error' | 'success';

  /**
   * Сообщение об ошибке.
   * @type {string}
   */
  errorMessage: string;
}
