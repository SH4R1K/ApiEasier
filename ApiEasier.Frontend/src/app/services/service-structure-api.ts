export interface Endpoint {
  route: string;
  type: string;
  isActive: boolean;
}

export interface Entity {
  name: string;
  isActive: boolean;
  structure: any;
  endpoints: Endpoint[];
}

export interface ApiServiceStructure {
  name: string;
  isActive: boolean;
  description: string;
  entities: Entity[];
}

export interface apiServiceShortStructure {
  name: string;
  isActive : boolean;
  description: string;
}

export interface EntityShort {
  name: string;
  isActive: boolean;
  structure: string;
}