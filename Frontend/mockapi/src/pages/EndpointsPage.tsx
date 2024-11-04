import { EndpointCard } from "@/components/Main/Endpoint";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
} from "@/components/ui/sheet";
import { Endpoint, EndpointPost } from "@/Entities/endpoint";
import { getEndpoints, deleteThisEndpoint, postEndpoint } from "@/services/endpointService";
import { useEffect, useState } from "react";

function EndpointsPage() {
  const [endpoints, setEndpoints] = useState([]);
  const [selectedEndpoint, setSelectedEndpoint] = useState<Endpoint | null>(
    null
  );
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [isSheetOpen, setIsSheetOpen] = useState(false);

  const [name, setName] = useState("");
  const [method, setMethod] = useState("GET");
  const [path, setPath] = useState("");
  const [responseObject, setResponseObject] = useState("");
  const [delay, setDelay] = useState(0);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await getEndpoints("/endpoint");
        setEndpoints(response);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  const openDialog = (endpoint: Endpoint) => {
    setSelectedEndpoint(endpoint);
    setIsDialogOpen(true);
  };

  const closeDialog = () => {
    setSelectedEndpoint(null);
    setIsDialogOpen(false);
  };

  const deleteEndpoint = async (endpoint: Endpoint) => {
    try {
      await deleteThisEndpoint("/endpoint?endpointID=" + endpoint.id);
      setEndpoints(
        endpoints.filter((endpoint: Endpoint) => endpoint.id !== endpoint.id)
      );
    } catch (error) {
      console.error("Error deleting endpoint:", error);
    }
  };

  const createEndpoint = async () => {
    try {
      let methodNumber: number = 0;
      switch (method) {
        case "GET":
          methodNumber = 0;
          break;
        case "POST":
          methodNumber = 1;
          break;
        case "PUT":
          methodNumber = 2;
          break;
        case "DELETE":
          methodNumber = 3;
          break;
        default:
          methodNumber = 0;
          break;
      }

      const endpointData: EndpointPost = {
        name: name,
        path: path,
        method: methodNumber,
        responseObject: responseObject,
        delay: delay,
        shouldFail: false,
        randomizeResponse: false,
      };
      if (endpointData.responseObject === "") {
        endpointData.responseObject = null;
      }
      await postEndpoint("/endpoint", endpointData);
    } catch (error) {
      console.error("Error creating endpoint:", error);
    }
  };

  return (
    <div className="flex flex-col items-center pt-8">
      <h1 className="text-3xl font-bold">Your Endpoints</h1>
      <Button onClick={() => setIsSheetOpen(true)} className="mt-4">
        Create New Endpoint
      </Button>
      <div className="grid grid-flow-row-dense grid-cols-3 grid-rows-3">
        {endpoints.map((endpoint: Endpoint) => (
          <div key={endpoint.id} className="p-4">
            <EndpointCard
              endpoint={endpoint}
              onUpdateClick={() => openDialog(endpoint)}
              onDeleteClick={() => deleteEndpoint(endpoint)}
            />
          </div>
        ))}
      </div>
      <Sheet open={isSheetOpen} onOpenChange={setIsSheetOpen}>
        <SheetContent>
          <SheetHeader>
            <SheetTitle>Create New Endpoint</SheetTitle>
            <SheetDescription>Create a new endpoint</SheetDescription>
          </SheetHeader>
          <div className="space-y-4 pb-4">
            <div>
              <p className="text-xs">Endpoint Name</p>
              <Input
                type="text"
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="Name"
              />
            </div>

            <div>
              <p className="text-xs">Endpoint Path</p>
              <Input
                type="text"
                value={path.startsWith("/") ? path : `/${path}`}
                onChange={(e) =>
                  setPath(
                    e.target.value.startsWith("/")
                      ? e.target.value
                      : `/${e.target.value}`
                  )
                }
              />
            </div>

            <div>
              <p className="text-xs">Method</p>
              <Select value={method} onValueChange={setMethod}>
                <SelectTrigger className="w-24">
                  <SelectValue>{method}</SelectValue>
                </SelectTrigger>
                <SelectContent>
                  <SelectGroup>
                    <SelectItem value="GET">GET</SelectItem>
                    <SelectItem value="POST">POST</SelectItem>
                    <SelectItem value="PUT">PUT</SelectItem>
                    <SelectItem value="DELETE">DELETE</SelectItem>
                  </SelectGroup>
                </SelectContent>
              </Select>
            </div>

            <div>
              <p className="text-xs">Response Object ID</p>
              <Input
                value={responseObject}
                onChange={(e) => setResponseObject(e.target.value)}
                type="text"
                placeholder="9dcebc0c-1e2e-413c-bee2-dbe1f7dfebe2"
              />
            </div>

            <div>
              <p className="text-xs">Delay in ms</p>
              <Input
                value={delay}
                onChange={(e) => setDelay(Number.parseInt(e.target.value))}
                type="number"
                placeholder="Delay"
              />
            </div>
          </div>
          <SheetFooter>
            <SheetClose>
              <Button onClick={() => {createEndpoint(); setIsSheetOpen(false);}}>
                Create Endpoint
              </Button>
            </SheetClose>
          </SheetFooter>
        </SheetContent>
      </Sheet>
      {/* Dialog at the page level for centering and overlay */}
      <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Update {selectedEndpoint?.name}</DialogTitle>
            <DialogDescription>Update the endpoint</DialogDescription>
          </DialogHeader>
          {selectedEndpoint && (
            <div>
              <Input
                type="text"
                placeholder="Name"
                defaultValue={selectedEndpoint.name}
              />
              <Input
                type="text"
                placeholder="Path"
                defaultValue={selectedEndpoint.path}
              />
              <Select>
                <SelectTrigger className="w-16">
                  <SelectValue>
                    {["GET", "POST", "PUT", "DELETE"][selectedEndpoint.method]}
                  </SelectValue>
                </SelectTrigger>
                <SelectContent>
                  <SelectGroup>
                    <SelectItem value="GET">GET</SelectItem>
                    <SelectItem value="POST">POST</SelectItem>
                    <SelectItem value="PUT">PUT</SelectItem>
                    <SelectItem value="DELETE">DELETE</SelectItem>
                  </SelectGroup>
                </SelectContent>
              </Select>
              <Input
                type="text"
                placeholder="Response Object"
                defaultValue={selectedEndpoint.responseObject}
              />
              <Input
                type="number"
                placeholder="Delay"
                defaultValue={selectedEndpoint.delay}
              />
              <Button onClick={closeDialog}>Update</Button>
            </div>
          )}
        </DialogContent>
      </Dialog>
    </div>
  );
}

export default EndpointsPage;
