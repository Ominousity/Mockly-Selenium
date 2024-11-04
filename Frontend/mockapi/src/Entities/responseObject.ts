/* eslint-disable @typescript-eslint/no-explicit-any */ // Disabling this rule to allow for any type in ObjectType interface
// Base interface to represent any object type, with Value and TypeName properties.
interface ObjectType {
    value: any; // Could be a primitive, an array, or an object
    typeName: string;
}

// Recursive interface for nested object types within ObjectType, if the value is a dictionary
interface NestedObjectType {
    [key: string]: ObjectType | NestedObjectType;
}

// Interface for representing lists within ObjectType
interface ListObjectType {
    value: ObjectType[]; // Array of ObjectTypes
    typeName: string;
}

// Interface for the data dictionary in ResponseObject
interface ResponseData {
    [key: string]: ObjectType | NestedObjectType | ListObjectType;
}

// Main ResponseObject interface
export interface ResponseObject {
    id: string; // Representing Guid as string
    data: ResponseData;
}