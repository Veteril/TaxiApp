
import {AxiosResponse} from 'axios';
import { OrderCreate } from "../models/OrderCreate";
import { OrderRead } from "../models/OrderRead";
import $apiOrder from "../http/orders";


export default class OrderService {
  
    static async createOrder(status: number, destination: string, address: string) : Promise<AxiosResponse<OrderRead>> {
        return $apiOrder.post('api/orders', {status,destination,address})
    }
}