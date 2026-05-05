import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { CarPart } from "../types/CarPart";
import type { CarPartDto } from "../types/CarPartDto";

type UpdateCarPartRequest = {
  id: string;
  carPart: CarPartDto;
};

export const apiSlice = createApi({
  reducerPath: "api",

  tagTypes: ["CarParts"],

  baseQuery: fetchBaseQuery({ baseUrl: "http://localhost:5274/api/" }),

  endpoints: (builder) => ({
    getCarParts: builder.query<CarPart[], void>({
      query: () => "carpart",
      providesTags: ["CarParts"],
    }),

    getCarPartById: builder.query<CarPart, string>({
      query: (id) => `carpart/${id}`,
    }),

    deleteCarPart: builder.mutation<void, string>({
      query: (id) => ({
        url: `carpart/${id}`,
        method: "DELETE",
      }),
      invalidatesTags: ["CarParts"],
    }),

    createCarPart: builder.mutation<CarPart, CarPartDto>({
      query: (newCarPart) => ({
        url: "carpart",
        method: "POST",
        body: newCarPart,
      }),
      invalidatesTags: ["CarParts"],
    }),

    updateCarpart: builder.mutation<void, UpdateCarPartRequest>({
      query: ({ id, carPart }) => ({
        url: `carpart/${id}`,
        method: "PUT",
        body: carPart,
      }),
      invalidatesTags: ["CarParts"],
    }),
  }),
});

export const {
  useGetCarPartsQuery,
  useGetCarPartByIdQuery,
  useDeleteCarPartMutation,
  useCreateCarPartMutation,
  useUpdateCarpartMutation,
} = apiSlice;
