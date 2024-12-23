export interface RegisterRequest {
    email: string;
    password: string;
}

export interface RegisterResponse {
    type: string;
    title: string;
    status: number;
    detail: string;
    errors?: { [key: string]: string[] }; // Mappa con eventuali errori restituiti dal server
    additionalProp1?: string;
    additionalProp2?: string;
    additionalProp3?: string;
}