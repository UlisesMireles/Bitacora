import { UnidadesNegocio } from "./UnidadesNegicio";

export class RespUnidadesNegocio {
  resul: string;
  unidadesNegocio: UnidadesNegocio[];

  constructor(resul: string, unidadesNegocio: UnidadesNegocio[]) {
    this.resul = resul;
    this.unidadesNegocio = unidadesNegocio;
  }
}
