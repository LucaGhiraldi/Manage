export interface DocumentiData {
    id: number;
    titolo: string;
    descrizione: string;
    dataCreazioneDocumento: Date;
    dataInserimentoDocumento: Date;
    utenteId: string;
    categoriaDocumentiId: number;
    sottoCategoriaDocumentiId: number;
    nomeCategoriaDocumenti: string;
    nomeSottoCategoriaDocumenti: string;
    idFiles: number[];
    nomiFiles: string[];
}

export interface DocumentiDataValue {
    id: number;
    titolo: string;
    descrizione: string;
    dataCreazioneDocumento: Date;
    dataInserimentoDocumento: Date;
    sottoCategoriaDocumentiId: number;
    idFiles: number[];
}

export interface DocumentiFilter {
    Titolo?: string;
    Descrizione?: string;
    DataCreazioneDocumento?: Date;
    DataInserimentoDocumento?: Date;
    // CategoriaDocumentiId?: number;
    SottoCategoriaId?: number;
}

export interface DocumentiCreateData {
    Titolo: string;
    Descrizione: string;
    DataCreazioneDocumento: Date;
    SottoCategoriaDocumentiId: number;
}