import { useEffect, useState } from "react";
import { v4 as uuidv4 } from "uuid";
import { Endpoint } from "./Entities/endpoint";
import {
  deleteThisEndpoint,
  getEndpoints,
  postEndpoint,
} from "./services/endpointService";

function App() {
  const [endpoints, setEndpoints] = useState<Endpoint[]>([]);
  const [newEndpoint, setNewEndpoint] = useState<Omit<Endpoint, "id">>({
    name: "",
    method: 0,
    path: "",
    mockResponse: "",
    delay: 0,
    shouldFail: false,
  });

  const addEndpoint = async () => {
    let uuid = uuidv4();
    setEndpoints([...endpoints, { ...newEndpoint, id: uuid }]);
    await postEndpoint(
      `/api/Endpoint?Id=${uuid}&Name=${newEndpoint.name}&Path=${newEndpoint.path}&Method=${newEndpoint.method}&MockResponse=${newEndpoint.mockResponse}`,
      { ...newEndpoint, id: uuid }
    );
    setNewEndpoint({
      name: "",
      method: 0,
      path: "",
      mockResponse: "",
      delay: 0,
      shouldFail: false,
    });
  };

  const deleteEndpoint = async (id: string) => {
    setEndpoints(endpoints.filter((endpoint) => endpoint.id !== id));
    await deleteThisEndpoint("/api/Endpoint?endpointID=" + id);
  };

  useEffect(() => {
    const fetchData = async () => {
      const result = await getEndpoints("/api/Endpoint");
      setEndpoints(result);
    };

    fetchData();
  }, []);

  return (
    <div className="container mx-auto py-8 px-4">
      <div className="space-y-8">
        <div className="bg-white shadow-md rounded-lg p-6">
          <h2 className="text-2xl font-bold mb-4">Add New Endpoint</h2>
          <div className="space-y-4">
            <div>
              <label
                htmlFor="name"
                className="block text-sm font-medium text-gray-700"
              >
                Endpoint Name
              </label>
              <input
                type="text"
                id="name"
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
                value={newEndpoint.name}
                onChange={(e) =>
                  setNewEndpoint({ ...newEndpoint, name: e.target.value })
                }
                placeholder="Mock API"
              />
            </div>
            <div>
              <label
                htmlFor="path"
                className="block text-sm font-medium text-gray-700"
              >
                Endpoint Path
              </label>
              <input
                type="text"
                id="path"
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
                value={newEndpoint.path}
                onChange={(e) =>
                  setNewEndpoint({ ...newEndpoint, path: e.target.value })
                }
                placeholder="/api/example"
              />
            </div>
            <div>
              <label
                htmlFor="method"
                className="block text-sm font-medium text-gray-700"
              >
                HTTP Method
              </label>
              <select
                id="method"
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
                value={newEndpoint.method}
                onChange={(e) =>
                  setNewEndpoint({
                    ...newEndpoint,
                    method: parseInt(e.target.value),
                  })
                }
              >
                <option value={0}>GET</option>
                <option value={1}>POST</option>
                <option value={2}>PATCH</option>
                <option value={3}>DELETE</option>
              </select>
            </div>
            <div>
              <label
                htmlFor="response"
                className="block text-sm font-medium text-gray-700"
              >
                Response
              </label>
              <textarea
                id="response"
                rows={3}
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
                value={newEndpoint.mockResponse}
                onChange={(e) =>
                  setNewEndpoint({ ...newEndpoint, mockResponse: e.target.value })
                }
                placeholder="{'key': 'value'}"
              ></textarea>
            </div>
            <div>
              <label
                htmlFor="delay"
                className="block text-sm font-medium text-gray-700"
              >
                Delay (ms)
              </label>
              <input
                type="number"
                id="delay"
                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
                value={newEndpoint.delay}
                onChange={(e) =>
                  setNewEndpoint({
                    ...newEndpoint,
                    delay: parseInt(e.target.value),
                  })
                }
                min={0}
              />
            </div>
            <div className="flex items-center">
              <input
                id="shouldFail"
                type="checkbox"
                className="rounded border-gray-300 text-indigo-600 shadow-sm focus:border-indigo-300 focus:ring focus:ring-offset-0 focus:ring-indigo-200 focus:ring-opacity-50"
                checked={newEndpoint.shouldFail}
                onChange={(e) =>
                  setNewEndpoint({
                    ...newEndpoint,
                    shouldFail: e.target.checked,
                  })
                }
              />
              <label
                htmlFor="shouldFail"
                className="ml-2 block text-sm text-gray-900"
              >
                Should Fail
              </label>
            </div>
            <button
              className="w-full py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
              onClick={addEndpoint}
            >
              Add Endpoint
            </button>
          </div>
        </div>
        <div className="bg-white shadow-md rounded-lg overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Name
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Path
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Method
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Response
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Delay (ms)
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Should Fail
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Actions
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {endpoints.map((endpoint) => (
                <tr key={endpoint.id}>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {endpoint.name}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {endpoint.path}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {endpoint.method}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {endpoint.mockResponse}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {endpoint.delay}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {endpoint.shouldFail ? "Yes" : "No"}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                    <button
                      className="text-red-600 hover:text-red-900"
                      onClick={() => deleteEndpoint(endpoint.id)}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

export default App;
