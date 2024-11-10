/* eslint-disable @typescript-eslint/no-explicit-any */
import type { ResponseObject } from "@/Entities/responseObject";
import { Card, CardContent, CardHeader, CardTitle } from "../ui/card";
import { Button } from "../ui/button";

type ResponseObjectCardProps = {
  responseObject: ResponseObject;
  onUpdateClick: () => void;
  onDeleteClick: () => void;
};

const simplifyTypeName = (typeName: string) => {
  if (typeName.includes("System.Collections.Generic.List")) {
    return "List";
  }if (typeName.includes("System.Collections.Generic.Dictionary")) {
    return "Dictionary";
  }
    // Extract just the type name (e.g., "System.Int32" -> "Int32")
    const parts = typeName.split('.');
    return parts[parts.length - 1]; // Get the last part
};

// Recursive component to render nested data fields
// @typescript-eslint/no-explicit-any
// biome-ignore lint/suspicious/noExplicitAny: <explanation>
const renderDataField = (dataField: any) => {
  if (dataField && typeof dataField === "object") {
    if ("value" in dataField && Array.isArray(dataField.value)) {
      // Handle arrays (lists)
      return (
        <div>
          <p>Type: {simplifyTypeName(dataField.typeName)}</p>
          <ul>
            {/* biome-ignore lint/suspicious/noExplicitAny: <explanation> */}
              {dataField.value.map((item: any) => (
              <li key={item.id || JSON.stringify(item)}>{renderDataField(item)}</li>
            ))}
          </ul>
        </div>
      );
    }

    if ("typeName" in dataField) {
      // Display the simplified type name
      const typeLabel = simplifyTypeName(dataField.typeName);

      return (
        <div>
          <p>Type: {typeLabel}</p>
          <div>{renderDataField(dataField.value)}</div>
        </div>
      );
    }

    // Handle nested dictionaries
    return (
      <div>
        {Object.keys(dataField).map((key) => (
          <div key={key} className="pl-4">
            <strong>{key}:</strong> {renderDataField(dataField[key])}
          </div>
        ))}
      </div>
    );
  }

  // For primitive values, display directly
  return <p>{String(dataField)}</p>;
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
        {Object.keys(responseObject.data).map((key) => {
          const dataField = responseObject.data[key];

          return (
            <div key={key} className="p-2">
              <h3 className="text-lg font-bold">{key}</h3>
              {renderDataField(dataField)}
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
