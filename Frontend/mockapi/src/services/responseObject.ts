import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:5220/api",
    headers: {
        "Content-Type": "application/json",
    },
});

export const getData = async (endpoint: string) => {
    try {
        const response = await api.get(endpoint);
        return response.data;
    } catch (error) {
        console.error("Error fetching data:", error);
        throw error;
    }
};