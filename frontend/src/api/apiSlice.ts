import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import type { CarPart } from "../types/CarPart";

export const apiSlice = createApi({
  reducerPath: "api",

  baseQuery: fetchBaseQuery({ baseUrl: "http://localhost:5274/api/" }),

  endpoints: (builder) => ({
    getCarPart: builder.query<CarPart[], void>({
      query: () => "carpart",
    }),
  }),
});

export const { useGetCarPartQuery } = apiSlice;
