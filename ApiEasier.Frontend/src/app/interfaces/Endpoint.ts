/**
 * Интерфейс, представляющий конечную точку (endpoint).
 *
 * @remarks
 * Этот интерфейс используется для описания маршрута, типа и статуса активности конечной точки.
 *
 * @type {{ route: string, type: string, isActive: boolean }}
 * @default { route: '', type: '', isActive: false }
 * @memberof Component
 */
export interface Endpoint {
  /**
   * Маршрут конечной точки.
   * @type {string}
   */
  route: string;

  /**
   * Тип конечной точки (например, GET, POST).
   * @type {string}
   */
  type: string;

  /**
   * Флаг активности конечной точки.
   * @type {boolean}
   * @default false
   */
  isActive: boolean;
}
