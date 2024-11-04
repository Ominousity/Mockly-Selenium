import { Endpoint } from "@/Entities/endpoint";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../ui/card";
import { Button } from "../ui/button";


type EndpointCardProps = {
    endpoint: Endpoint;
    onUpdateClick: () => void;
    onDeleteClick: () => void;
};


export function EndpointCard({ endpoint, onUpdateClick, onDeleteClick }: EndpointCardProps) {
  const httpMethods = ["GET", "POST", "PUT", "DELETE"];

  return (
    <Card>
      <CardHeader>
        <CardTitle>{endpoint.name}</CardTitle>
        <CardDescription>{endpoint.path}</CardDescription>
      </CardHeader>
      <CardContent>
        <p>{httpMethods[endpoint.method]}</p>
        <p>{endpoint.responseObject ?? 'Empty Response'}</p>
        <p>{endpoint.delay}</p>
        <div className="flex flex-row justify-evenly">
            <Button onClick={onDeleteClick}>Delete</Button>
            <Button onClick={onUpdateClick}>Update</Button>
        </div>
      </CardContent>
    </Card>
  );
}
