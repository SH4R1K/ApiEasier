/**
 * Интерфейс, представляющий краткую информацию о сущности.
 *
 * @remarks
 * Этот интерфейс используется для описания основных характеристик сущности.
 *
 * @type {{ name: string, isActive: boolean, structure: string }}
 * @default { name: '', isActive: false, structure: '' }
 * @memberof Component
 */
export interface EntityShort {
  /**
   * Название сущности.
   * @type {string}
   */
  name: string;

  /**
   * Флаг активности сущности.
   * @type {boolean}
   * @default false
   */
  isActive: boolean;

  /**
   * Структура сущности в виде строки.
   * @type {string}
   */
  structure: string;
}
