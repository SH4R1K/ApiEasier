import { Endpoint } from "./Endpoint";


export interface Entity {
  name: string;
  isActive: boolean;
  structure: any;
  endpoints: Endpoint[];
}
