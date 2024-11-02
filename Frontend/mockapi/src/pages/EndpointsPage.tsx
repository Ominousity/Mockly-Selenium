import { EndpointCard } from '@/components/Main/Endpoint';
import { Endpoint } from '@/Entities/endpoint';
import { getEndpoints } from '@/services/endpointService';
import { useEffect, useState } from 'react';

function EndpointsPage(){

    const [endpoints, setEndpoints] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await getEndpoints('/endpoint');
                setEndpoints(response);
            } catch (error) {
                console.error("Error fetching data:", error);
            }
        };

        fetchData();
    }, []);

    return (
        <div className='flex flex-col items-center pt-8'>
            <h1 className='text-3xl font-bold'>Home</h1>
            <div className='grid grid-flow-row-dense grid-cols-3 grid-rows-3'>
                {endpoints.map((endpoint: Endpoint) => (
                    <div key={endpoint.id} className='p-4'>
                        <EndpointCard {...endpoint}/>
                    </div>
                ))}
            </div>
        </div>
    )
}

export default EndpointsPage;