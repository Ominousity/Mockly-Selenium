import { ResponseObject } from "@/Entities/responseObject";
import { Card, CardContent, CardHeader, CardTitle } from "../ui/card";
import { Button } from "../ui/button";

type ResponseObjectCardProps = {
  responseObject: ResponseObject;
  onUpdateClick: () => void;
  onDeleteClick: () => void;
};

export function ResponseObjectCard({
  responseObject,
  onUpdateClick,
  onDeleteClick,
}: ResponseObjectCardProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>{responseObject.id}</CardTitle>
      </CardHeader>
      <CardContent>
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
                            <pre>{JSON.stringify(item.value, null, 2)}</pre>
                          ) : (
                            String(item.value)
                          )}
                        </li>
                      ))}
                    </ul>
                  ) : (
                    // Nested dictionary object
                    <pre>{JSON.stringify(dataField.value, null, 2)}</pre>
                  )
                ) : (
                  // Primitive or directly serializable value
                  <p>{String(dataField.value)}</p>
                )}
              </div>
            </div>
          );
        })}
        <div className="flex flex-row justify-evenly">
          <Button onClick={onDeleteClick}>Delete</Button>
          <Button onClick={onUpdateClick}>Update</Button>
        </div>
      </CardContent>
    </Card>
  );
}
