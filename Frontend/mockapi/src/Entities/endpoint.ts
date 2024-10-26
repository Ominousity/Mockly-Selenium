export interface Endpoint {
    id: string;
    name: string;
    path: string;
    method: number;
    mockResponse: string;
    delay: number;
    shouldFail: boolean;
}