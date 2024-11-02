export interface Endpoint {
    id: string;
    name: string;
    path: string;
    method: number;
    responseObject: string;
    delay: number;
    shouldFail: boolean;
    randomizeResponse: boolean;
}