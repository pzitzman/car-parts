import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { CarPart } from "../types/CarPart";

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
  }),
});

export const {
  useGetCarPartsQuery,
  useGetCarPartByIdQuery,
  useDeleteCarPartMutation,
} = apiSlice;
