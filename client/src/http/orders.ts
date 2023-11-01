import axios from "axios";
import { config } from "process";

export const API_URL_ORDER = 'http://localhost:5266'

const $apiOrder = axios.create({
    withCredentials: true,
    baseURL: API_URL_ORDER
})

$apiOrder.interceptors.request.use((config) => {
    config.headers.Authorization =  'Bearer ' + localStorage.getItem('token');
    return config;
})

export default $apiOrder; 