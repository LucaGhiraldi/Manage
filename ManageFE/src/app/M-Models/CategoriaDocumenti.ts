export interface SottoCategoriaDocumentiData {
    id: number;
    nomeSottoCategoria: string;
    descrizioneSottoCategoria: string;
    dataInserimentoSottoCategoria: Date;
}

export interface CategoriaDocumentiData {
    id: number;
    nomeCategoria: string;
    descrizioneCategoria: string;
    dataInserimentoCategoria: Date;
    isPredefinita: boolean;
    utenteId: string;

    sottoCategorie: SottoCategoriaDocumentiData[];
}

// L'oggetto che unisce categoria e sotto-categoria
export interface CategoriaWithSottoCategoria {
    categoriaId: number;
    categoriaNome: string;
    sottoCategoriaId: number;
    sottoCategoriaNome: string;
}

export interface CategoriaDocumentiUpdateData {
    id: number;
    nomeCategoria: string;
    descrizioneCategoria: string;
    dataInserimentoCategoria: Date;
    // isPredefinita: boolean;

    sottoCategorie: SottoCategoriaDocumentiData[];
}
export interface CategoriaDocumentiCreateData {
    nomeCategoria: string;
    descrizioneCategoria: string;
    dataInserimentoCategoria: Date;
    // isPredefinita: boolean;

    sottoCategorie: SottoCategoriaDocumentiData[];
}