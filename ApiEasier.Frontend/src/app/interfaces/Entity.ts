import { Endpoint } from "./Endpoint";

/**
 * Интерфейс, представляющий сущность.
 *
 * @remarks
 * Этот интерфейс используется для описания сущности, включая её структуру и связанные конечные точки.
 *
 * @type {{ name: string, isActive: boolean, structure: any, endpoints: Endpoint[] }}
 * @default { name: '', isActive: false, structure: {}, endpoints: [] }
 * @memberof Component
 */
export interface Entity {
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
   * Структура сущности.
   * @type {any}
   */
  structure: any;

  /**
   * Массив конечных точек, связанных с сущностью.
   * @type {Endpoint[]}
   */
  endpoints: Endpoint[];
}
