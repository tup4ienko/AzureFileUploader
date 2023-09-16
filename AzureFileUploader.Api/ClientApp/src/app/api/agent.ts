import axios, { AxiosError, AxiosResponse } from 'axios';
import {IUploadFileFormInputs} from "../../models/uploadFile.ts";

axios.defaults.baseURL = import.meta.env.VITE_API_URL;

axios.defaults.timeout = 10000

axios.interceptors.response.use(
    async (response: AxiosResponse) => {
        return response;
    },
    (error: AxiosError) => {
        return Promise.reject(error);
    },
);


const User = {
    uploadFile: (data: IUploadFileFormInputs) => {
        const formData = new FormData();
        formData.append('Email', data.email);
        formData.append('Document', data.document);
        return axios.post<void>('/user/upload-document', formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
        });
    },
}

const agent = {
    User
};

export default agent;
