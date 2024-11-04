import axios from "axios";
import { Endpoint, EndpointPost } from "../Entities/endpoint";

const api = axios.create({
    baseURL: "http://localhost:5220/api",
    headers: {
        "Content-Type": "application/json",
    },
});

export const getEndpoints = async (endpoint: string) => {
    try {
        const response = await api.get(endpoint);
        return response.data;
    } catch (error) {
        console.error("Error fetching data:", error);
        throw error;
    }
};

export const postEndpoint = async (endpoint: string, data: EndpointPost) => {
    try {
        const response = await api.post(endpoint, data);
        return response.data;
    } catch (error) {
        console.error("Error posting data:", error);
        throw error;
    }
};

export const putEndpoint = async (endpoint: string, data: Endpoint) => {
    try {
        const response = await api.put(endpoint, data);
        return response.data;
    } catch (error) {
        console.error("Error putting data:", error);
        throw error;
    }
};

export const patchEndpoint = async (endpoint: string, data: string) => {
    try {
        const response = await api.patch(endpoint, data);
        return response.data;
    } catch (error) {
        console.error("Error patching data:", error);
        throw error;
    }
};

export const deleteThisEndpoint = async (endpoint: string) => {
    try {
        const response = await api.delete(endpoint);
        return response.data;
    } catch (error) {
        console.error("Error deleting data:", error);
        throw error;
    }
};