import { AllCommunityModule, type ColDef } from "ag-grid-community";
import "./App.css";
import { useGetCarPartQuery } from "./api/apiSlice";
import type { CarPart } from "./types/CarPart";
import { AgGridProvider, AgGridReact } from "ag-grid-react";

const modules = [AllCommunityModule];

function formatDate(dateString: string | undefined) {
  if (!dateString) {
    return "";
  }

  return new Date(dateString).toLocaleString();
}

const columnDefs: ColDef<CarPart>[] = [
  { field: "id", headerName: "ID", hide: true },
  { field: "name", headerName: "Name" },
  { field: "partNumber", headerName: "Part Number" },
  { field: "description", headerName: "Desciption", flex: 2 },
  {
    field: "createdAt",
    headerName: "Created At",
    valueFormatter: (params) => formatDate(params.value),
  },
  {
    field: "updatedAt",
    headerName: "Updated At",
    valueFormatter: (params) => formatDate(params.value),
  },
];

const defaultColDef: ColDef<CarPart> = {
  sortable: true,
  filter: true,
  resizable: true,
  flex: 1,
};

function App() {
  const { data: carParts, isLoading, isError, error } = useGetCarPartQuery();

  if (isLoading) {
    return <p>Loading car parts</p>;
  }

  if (isError) {
    return (
      <div>
        <h1>Error laoding car parts</h1>
        <pre>{JSON.stringify(error, null, 2)}</pre>
      </div>
    );
  }

  return (
    <main>
      <h1>Car parts</h1>
      <AgGridProvider modules={modules}>
        <div style={{ height: 500 }}>
          <AgGridReact
            rowData={carParts}
            columnDefs={columnDefs}
            defaultColDef={defaultColDef}
            pagination={true}
            paginationPageSize={10}
          />
        </div>
      </AgGridProvider>
    </main>
  );
}

export default App;
