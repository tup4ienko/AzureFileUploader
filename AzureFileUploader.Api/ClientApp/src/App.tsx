import {TextField, Collapse, Alert, IconButton} from "@mui/material";
import {useEffect, useState} from "react";
import {Controller, SubmitHandler, useForm} from "react-hook-form";
import {Close, FileUpload} from "@mui/icons-material";
import {IUploadFileFormInputs} from "./models/uploadFile.ts";
import {LoadingButton} from "@mui/lab";
import {useFilePicker} from "use-file-picker";
import {useMutation} from "react-query";
import agent from "./app/api/agent.ts";
import toast, {Toaster} from "react-hot-toast";

const App = () => {
    const uploadFile = useMutation({
        mutationFn: (params: IUploadFileFormInputs) => {
            return agent.User.uploadFile(params);
        },
        onError: (err: Error) => {
            toast.error(err.message);
        },
        onSuccess: () => {
            setOpenBannerAlert(false);
            clear();
            reset();
            toast.success("Success")
        },
    });

    const [openBannerAlert, setOpenBannerAlert] = useState<boolean>(false);

    const {openFilePicker, filesContent, loading, clear, plainFiles} = useFilePicker({
        readAs: "DataURL",
        multiple: false,
        limitFilesConfig: {max: 1},
    });

    const {
        handleSubmit,
        control,
        reset,
        formState: {errors},
        setValue,
    } = useForm<IUploadFileFormInputs>({
        mode: "onSubmit",
        defaultValues: {
            email: "",
            document: "",
        },
    });

    const onSubmit: SubmitHandler<IUploadFileFormInputs> = async (data) => {
        const allowedDocumentTypes = ["application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"];

        if (!data.document || !allowedDocumentTypes.includes(data.document.type)) {
            setOpenBannerAlert(true);
        } else {

            await uploadFile.mutateAsync(data)
        }

    };

    useEffect(() => {
        if (filesContent.length > 0) {
            setValue("document", plainFiles[0]);
        }
    }, [filesContent, setValue]);

    return (
        <div className="bg-amber- flex justify-center">
            <Toaster />
            <div className="bg-white p-6 mr-3.5 ml-3.5 pl-6 pt-6 rounded-lg drop-shadow-lg w-[25rem] mt-28 relative">
                <form className="flex flex-col" onSubmit={handleSubmit(onSubmit)}>
                    <span className="text-3xl font-bold text-center">Upload File</span>
                    <div className="flex flex-col mt-9 w-full mb-8">
                        <div className="flex flex-col">
                            <div className="flex flex-col mb-4">
                                <label>Email</label>
                                <Controller
                                    name="email"
                                    control={control}
                                    rules={{
                                        required: true,
                                        pattern: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/i
                                    }}
                                    render={({field}) => (
                                        <TextField
                                            {...field}
                                            placeholder='user123@mail.com'
                                            error={errors.email ? true : false}
                                            helperText={errors.email && 'Invalid email address format'}
                                        />
                                    )}
                                />
                            </div>
                            <div className="">
                                <Collapse in={openBannerAlert}>
                                    <Alert
                                        severity="error"
                                        action={
                                            <IconButton
                                                aria-label="close"
                                                color="inherit"
                                                size="small"
                                                onClick={() => {
                                                    setOpenBannerAlert(false);
                                                }}
                                            >
                                                <Close fontSize="inherit"/>
                                            </IconButton>
                                        }
                                        sx={{mb: 2}}
                                    >
                                        {filesContent.length === 0 ? "You need to upload a document" : "Documents must be in .doc or .docx format"}
                                    </Alert>
                                </Collapse>

                                <LoadingButton
                                    loading={loading}
                                    onClick={() => openFilePicker()}
                                    className="w-full mb-4"
                                    variant="contained"
                                    color="info"
                                    size="large"
                                    startIcon={<FileUpload sx={{color: "white"}}/>}
                                />
                                {filesContent.length > 0 && (
                                    <span>{filesContent[0].name}</span>
                                )}
                            </div>
                        </div>
                    </div>
                    <LoadingButton
                        loading={uploadFile.isLoading}
                        onClick={() => onSubmit}
                        className="w-full"
                        variant="contained"
                        color="success"
                        type="submit"
                    >
                        Send
                    </LoadingButton>
                </form>
            </div>
        </div>
    );
};

export default App;
