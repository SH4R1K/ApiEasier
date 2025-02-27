import { Entity } from "./Entity";

/**
 * Интерфейс, представляющий полную структуру API сервиса.
 *
 * @remarks
 * Этот интерфейс расширяет `apiServiceShortStructure`, добавляя массив сущностей.
 *
 * @type {{ name: string, isActive: boolean, description: string, entities: Entity[] }}
 * @default { name: '', isActive: false, description: '', entities: [] }
 * @memberof Component
 */
export interface ApiServiceStructure {
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

  /**
   * Массив сущностей, связанных с сервисом.
   * @type {Entity[]}
   */
  entities: Entity[];
}
