export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    tokenType: string;
    accessToken: string;
    expiresIn: number;
    refreshToken: number;
}