/**
 * Интерфейс, представляющий краткую структуру API сервиса.
 *
 * @remarks
 * Этот интерфейс используется для описания основных характеристик API сервиса.
 * Он включает в себя имя сервиса, его статус активности и краткое описание.
 *
 * @type {{ name: string, isActive: boolean, description: string }}
 * @default { name: '', isActive: false, description: '' }
 * @memberof Component
 */
export interface apiServiceShortStructure {
  /**
   * Название сервиса.
   * @type {string}
   */
  name: string;

  /**
   * Флаг активности сервиса.
   * @type {boolean}
   * @default false
   */
  isActive: boolean;

  /**
   * Описание сервиса.
   * @type {string}
   */
  description: string;
}
