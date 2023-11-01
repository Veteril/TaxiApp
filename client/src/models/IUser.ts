import { IClientRatings } from "./IClientRatings"

export interface IUser {
    id: string
    username: string
    phone: string
    email: string
    rating: number
    role: string
    clientRatingsDtos : IClientRatings[]
}