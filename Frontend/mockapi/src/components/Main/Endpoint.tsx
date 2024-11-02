import { Endpoint } from "@/Entities/endpoint";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "../ui/card";
import { Button } from "../ui/button";
import { Dialog, DialogContent, DialogDescription, DialogTitle, DialogTrigger } from "@radix-ui/react-dialog";
import { DialogHeader } from "../ui/dialog";

export function EndpointCard(endpoint: Endpoint) {

    const deleteEndpoint = async () => {};
    const httpMethods = ['GET', 'POST', 'PUT', 'DELETE'];

    return (
        <Card>
            <Dialog>
                <CardHeader>
                    <CardTitle>{endpoint.name}</CardTitle>
                    <CardDescription>{endpoint.path}</CardDescription>
                </CardHeader>
                <CardContent>
                    <p>{httpMethods[endpoint.method]}</p>
                    <p>{endpoint.responseObject}</p>
                    <p>{endpoint.delay}</p>
                    <Button onClick={deleteEndpoint}>Delete</Button>
                    <DialogTrigger>
                        <Button>Update</Button>
                    </DialogTrigger>
                </CardContent>
                <DialogContent>
                    <DialogHeader>
                        <DialogTitle>Update {endpoint.name}</DialogTitle>
                        <DialogDescription>Update the endpoint</DialogDescription>
                    </DialogHeader>
                    <div>
                        <input type='text' placeholder='Name' value={endpoint.name}/>
                        <input type='text' placeholder='Path' value={endpoint.path}/>
                        <select>
                            {httpMethods.map((method, index) => (
                                <option key={index} value={index}>{method}</option>
                            ))}
                        </select>
                        <input type='text' placeholder='Response Object' value={endpoint.responseObject}/>
                        <input type='number' placeholder='Delay' value={endpoint.delay}/>
                        <input type='checkbox' checked={endpoint.shouldFail}/>
                        <input type='checkbox' checked={endpoint.randomizeResponse}/>
                        <Button>Update</Button>
                    </div>
                </DialogContent>
            </Dialog>
        </Card>
    )
}