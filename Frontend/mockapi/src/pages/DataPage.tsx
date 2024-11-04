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
              <h2 className="text-xl font-bold">{responseObject.id}</h2>
              <div className="grid grid-flow-row-dense grid-cols-2 grid-rows-2">
                {responseObject.data ? (
                  <div className="grid grid-flow-row-dense grid-cols-2 grid-rows-2">
                    {Object.keys(responseObject.data).map((key: string) => {
                      const dataField = responseObject.data[key];

                      return (
                        <div key={key} className="p-2">
                          <h3 className="text-lg font-bold">{key}</h3>

                          {/* Display the type name */}
                          <p>{String(dataField.typeName ?? "No Type Available")}</p>

                          {/* Check if value is a nested object, array, or a primitive */}
                          <div>
                            {typeof dataField.value === "object" ? (
                              // If value is an object, check if it's an array or a nested object
                              Array.isArray(dataField.value) ? (
                                // Array of ObjectTypes
                                <ul>
                                  {dataField.value.map((item, index) => (
                                    <li key={index}>
                                      {typeof item.value === "object" ? (
                                        <pre>
                                          {JSON.stringify(item.value, null, 2)}
                                        </pre>
                                      ) : (
                                        String(item.value)
                                      )}
                                    </li>
                                  ))}
                                </ul>
                              ) : (
                                // Nested dictionary object
                                <pre>
                                  {JSON.stringify(dataField.value, null, 2)}
                                </pre>
                              )
                            ) : (
                              // Primitive or directly serializable value
                              <p>{String(dataField.value)}</p>
                            )}
                          </div>
                        </div>
                      );
                    })}
                  </div>
                ) : (
                  <p>No Data Available</p>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default DataPage;
