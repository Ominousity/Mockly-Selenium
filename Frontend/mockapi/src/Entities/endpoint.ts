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

export interface EndpointPost {
    name: string;
    path: string;
    method: number;
    responseObject: string | null;
    delay: number;
    shouldFail: boolean;
    randomizeResponse: boolean;
}