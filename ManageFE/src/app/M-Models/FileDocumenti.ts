export interface FileDocumentiData {
    id: number;
    nomeFile: string;
    estensioneFile: string;
    percorsoFile: string;
    dataInserimentoFile: Date;
    documentiId: number;
    isTemporary?: boolean;
}

export interface FileDocumentiDataValue {
    id: number;
    nomeFile: string;
    estensioneFile: string;
    dataInserimentoFile: Date;
    documentiId: number;
}