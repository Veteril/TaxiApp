import { IUser } from "./IUser";

export interface AuthResponse {
    id: string;
    username: string;
    token: string;
    refreshToken: string;
    role: string;
}