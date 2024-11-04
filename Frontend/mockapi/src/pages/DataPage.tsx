import { ResponseObjectCard } from "@/components/Main/ResponseObjectCard";
import { Button } from "@/components/ui/button";
import { ResponseObject } from "@/Entities/responseObject";
import { getData } from "@/services/responseObject";
import { useEffect, useState } from "react";

function DataPage() {
  const [data, setData] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await getData("/ResponseObject");
        setData(response);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="flex flex-col items-center pt-8">
      <h1 className="text-3xl font-bold">Your Endpoints</h1>
      <Button className="mt-4">Create new ResponseObject</Button>
      {data.length === 0 ? (
        <p>Loading...</p>
      ) : (
        <div className="grid grid-flow-row-dense grid-cols-3 grid-rows-3">
          {data.map((responseObject: ResponseObject) => (
            <div key={responseObject.id} className="p-4">
              <ResponseObjectCard
                responseObject={responseObject}
                onDeleteClick={() => console.log("Delete")}
                onUpdateClick={() => console.log("Update")}
              />
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default DataPage;
