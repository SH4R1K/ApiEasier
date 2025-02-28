import { Entity } from "./Entity";


export interface ApiServiceStructure {
  name: string;
  isActive: boolean;
  description: string;
  entities: Entity[];
}
