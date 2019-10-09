--
-- PostgreSQL database dump
--

-- Dumped from database version 10.5
-- Dumped by pg_dump version 10.5

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: Item; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Item" (
    "ItemId" integer NOT NULL,
    "Name" text NOT NULL
);


--
-- Name: Item2; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Item2" (
    "Item2Id" integer NOT NULL,
    "Name" text NOT NULL
);


--
-- Name: Item2_Item2Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Item2" ALTER COLUMN "Item2Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Item2_Item2Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Item_ItemId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."Item" ALTER COLUMN "ItemId" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Item_ItemId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: schemaversions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.schemaversions (
    schemaversionsid integer NOT NULL,
    scriptname character varying(255) NOT NULL,
    applied timestamp without time zone NOT NULL
);


--
-- Name: schemaversions_schemaversionsid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public.schemaversions_schemaversionsid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: schemaversions_schemaversionsid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public.schemaversions_schemaversionsid_seq OWNED BY public.schemaversions.schemaversionsid;


--
-- Name: schemaversions schemaversionsid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.schemaversions ALTER COLUMN schemaversionsid SET DEFAULT nextval('public.schemaversions_schemaversionsid_seq'::regclass);


--
-- Name: Item PK_Item; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Item"
    ADD CONSTRAINT "PK_Item" PRIMARY KEY ("ItemId");


--
-- Name: Item2 PK_Item2; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Item2"
    ADD CONSTRAINT "PK_Item2" PRIMARY KEY ("Item2Id");


--
-- Name: schemaversions PK_schemaversions_Id; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.schemaversions
    ADD CONSTRAINT "PK_schemaversions_Id" PRIMARY KEY (schemaversionsid);


--
-- Name: Item2 UQ_Item2_Name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Item2"
    ADD CONSTRAINT "UQ_Item2_Name" UNIQUE ("Name");


--
-- Name: Item UQ_Item_Name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Item"
    ADD CONSTRAINT "UQ_Item_Name" UNIQUE ("Name");


--
-- PostgreSQL database dump complete
--

