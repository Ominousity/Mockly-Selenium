import { Route, BrowserRouter as Router, Routes } from 'react-router-dom';
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { AppSidebar } from './components/Main/AppSidebar';
import EndpointsPage from './pages/EndpointsPage';
import DataPage from './pages/DataPage';
import SettingsPage from './pages/SettingsPage';

function App() {
  return (
    <Router>
      <SidebarProvider>
        <AppSidebar/>
        <main className="flex w-full min-h-screen">
          <div className="flex items-start">
            <SidebarTrigger />
          </div>
          <div className="flex flex-col items-center justify-start w-full">
            <Routes>
              <Route path="/Endpoints" element={<EndpointsPage />} />
              <Route path="/Data" element={<DataPage />} />
              <Route path="/Settings" element={<SettingsPage />} />
            </Routes>
          </div>
        </main>
      </SidebarProvider>
    </Router>
  );
}

export default App;
