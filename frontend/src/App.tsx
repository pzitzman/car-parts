import {
  AllCommunityModule,
  type ColDef,
  type ICellRendererParams,
} from "ag-grid-community";
import "./App.css";
import {
  useCreateCarPartMutation,
  useDeleteCarPartMutation,
  useGetCarPartsQuery,
} from "./api/apiSlice";
import type { CarPart } from "./types/CarPart";
import { AgGridProvider, AgGridReact } from "ag-grid-react";
import { useState } from "react";
import type { CarPartDto } from "./types/CarPartDto";

const modules = [AllCommunityModule];

function formatDate(dateString: string | undefined) {
  if (!dateString) {
    return "";
  }

  return new Date(dateString).toLocaleString();
}

const defaultColDef: ColDef<CarPart> = {
  sortable: true,
  filter: true,
  resizable: true,
  flex: 1,
};

function App() {
  const { data: carParts, isLoading, isError, error } = useGetCarPartsQuery();

  const [deleteCarPart, { isLoading: isDeleting }] = useDeleteCarPartMutation();

  const [createCarPart, { isLoading: isCreating }] = useCreateCarPartMutation();

  const [isCreateFormOpen, setIsCreateFormOpen] = useState(false);

  const [formData, setFormData] = useState<CarPartDto>({
    name: "",
    partNumber: "",
    description: "",
  });

  function handleInputChange(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;

    setFormData((currentFormData) => ({
      ...currentFormData,
      [name]: value,
    }));
  }

  async function handleCreate(e: React.SubmitEvent<HTMLFormElement>) {
    e.preventDefault();

    const dto: CarPartDto = {
      name: formData.name.trim(),
      partNumber: formData.partNumber.trim(),
      description: formData.description.trim(),
    };

    if (!dto.name || !dto.partNumber) {
      window.alert("Name und Teilnummer sind erforderlich.");
      return;
    }

    try {
      await createCarPart(dto).unwrap();

      setFormData({
        name: "",
        partNumber: "",
        description: "",
      });

      setIsCreateFormOpen(false);
    } catch (error) {
      console.error("Erstellen fehlgeschlagen:", error);
    }
  }

  async function handleDelete(id: string) {
    const confirmed = window.confirm(
      "Soll das Autoteil wirklich entfernt werden ?",
    );

    if (!confirmed) {
      return;
    }

    try {
      await deleteCarPart(id).unwrap();
    } catch (error) {
      console.error("Entfernen fehlgeschlagen!:", error);
    }
  }

  const columnDefs: ColDef<CarPart>[] = [
    { field: "id", headerName: "ID", hide: true },
    { field: "name", headerName: "Name" },
    { field: "partNumber", headerName: "Teilnummer" },
    { field: "description", headerName: "Beschreibung", flex: 2 },
    {
      field: "createdAt",
      headerName: "Erstellt am",
      valueFormatter: (params) => formatDate(params.value),
    },
    {
      field: "updatedAt",
      headerName: "Stand",
      valueFormatter: (params) => formatDate(params.value),
    },
    {
      headerName: "Aktion",
      sortable: false,
      filter: false,
      cellRenderer: (params: ICellRendererParams<CarPart>) => {
        if (!params.data) {
          return null;
        }

        const id = params.data.id;

        return (
          <button
            type="button"
            disabled={isDeleting}
            onClick={() => handleDelete(id)}
          >
            Entfernen
          </button>
        );
      },
    },
  ];

  if (isLoading) {
    return <p>Autoteile weren geladen</p>;
  }

  if (isError) {
    return (
      <div>
        <h1>Autoteile konnten nicht geladen werden</h1>
        <pre>{JSON.stringify(error, null, 2)}</pre>
      </div>
    );
  }

  return (
    <main>
      <h1>Autoteile</h1>
      <button type="button" onClick={() => setIsCreateFormOpen(true)}>
        Teil erstellen
      </button>

      {isCreateFormOpen && (
        <form onSubmit={handleCreate}>
          <input
            name="name"
            value={formData.name}
            onChange={handleInputChange}
            placeholder="Name"
          />

          <input
            name="partNumber"
            value={formData.partNumber}
            onChange={handleInputChange}
            placeholder="Teilnummer"
          />

          <input
            name="description"
            value={formData.description}
            onChange={handleInputChange}
            placeholder="Bescreibung"
          />

          <button type="submit" disabled={isCreating}>
            {isCreating ? "Erstellen..." : "Erstellen"}
          </button>

          <button type="button" onClick={() => setIsCreateFormOpen(false)}>
            Abbrechen
          </button>
        </form>
      )}

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
