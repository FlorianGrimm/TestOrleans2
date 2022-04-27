//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.15.10.0 (NJsonSchema v10.6.10.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

export class Client {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        this.http = http ? http : window as any;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    /**
     * @return Success
     */
    me(): Promise<User> {
        let url_ = this.baseUrl + "/api/Me";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processMe(_response);
        });
    }

    protected processMe(response: Response): Promise<User> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = User.fromJS(resultData200);
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<User>(null as any);
    }

    /**
     * @return Success
     */
    project(): Promise<Project[]> {
        let url_ = this.baseUrl + "/api/Me/Project";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processProject(_response);
        });
    }

    protected processProject(response: Response): Promise<Project[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(Project.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<Project[]>(null as any);
    }

    /**
     * @return Success
     */
    toDoAllAll(): Promise<ToDo[]> {
        let url_ = this.baseUrl + "/api/Me/ToDo";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processToDoAllAll(_response);
        });
    }

    protected processToDoAllAll(response: Response): Promise<ToDo[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(ToDo.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ToDo[]>(null as any);
    }

    /**
     * @return Success
     */
    projectGetAll(): Promise<Project[]> {
        let url_ = this.baseUrl + "/api/Project";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processProjectGetAll(_response);
        });
    }

    protected processProjectGetAll(response: Response): Promise<Project[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(Project.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<Project[]>(null as any);
    }

    /**
     * @param body (optional) 
     * @return Success
     */
    projectPost(body: Project | undefined): Promise<Project> {
        let url_ = this.baseUrl + "/api/Project";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processProjectPost(_response);
        });
    }

    protected processProjectPost(response: Response): Promise<Project> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = Project.fromJS(resultData200);
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<Project>(null as any);
    }

    /**
     * @return Success
     */
    projectGetOne(projectId: string): Promise<Project> {
        let url_ = this.baseUrl + "/api/Project/{projectId}";
        if (projectId === undefined || projectId === null)
            throw new Error("The parameter 'projectId' must be defined.");
        url_ = url_.replace("{projectId}", encodeURIComponent("" + projectId));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processProjectGetOne(_response);
        });
    }

    protected processProjectGetOne(response: Response): Promise<Project> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = Project.fromJS(resultData200);
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<Project>(null as any);
    }

    /**
     * @param body (optional) 
     * @return Success
     */
    projectPut(projectId: string, body: Project | undefined): Promise<Project> {
        let url_ = this.baseUrl + "/api/Project/{projectId}";
        if (projectId === undefined || projectId === null)
            throw new Error("The parameter 'projectId' must be defined.");
        url_ = url_.replace("{projectId}", encodeURIComponent("" + projectId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processProjectPut(_response);
        });
    }

    protected processProjectPut(response: Response): Promise<Project> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = Project.fromJS(resultData200);
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<Project>(null as any);
    }

    /**
     * @return Success
     */
    projectDelete(projectId: string): Promise<void> {
        let url_ = this.baseUrl + "/api/Project/{projectId}";
        if (projectId === undefined || projectId === null)
            throw new Error("The parameter 'projectId' must be defined.");
        url_ = url_.replace("{projectId}", encodeURIComponent("" + projectId));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "DELETE",
            headers: {
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processProjectDelete(_response);
        });
    }

    protected processProjectDelete(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    /**
     * @return Success
     */
    toDoAllAll22(): Promise<ToDo[]> {
        let url_ = this.baseUrl + "/api/ToDo";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processToDoAllAll22(_response);
        });
    }

    protected processToDoAllAll22(response: Response): Promise<ToDo[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(ToDo.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ToDo[]>(null as any);
    }

    /**
     * @param body (optional) 
     * @return Success
     */
    toDoPOSTPOST(body: ToDo | undefined): Promise<void> {
        let url_ = this.baseUrl + "/api/ToDo";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processToDoPOSTPOST(_response);
        });
    }

    protected processToDoPOSTPOST(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    /**
     * @return Success
     */
    toDoGETGET(projectId: string, toDoId: string): Promise<ToDo> {
        let url_ = this.baseUrl + "/api/ToDo/{projectId}/{todoId}";
        if (projectId === undefined || projectId === null)
            throw new Error("The parameter 'projectId' must be defined.");
        url_ = url_.replace("{projectId}", encodeURIComponent("" + projectId));
        if (toDoId === undefined || toDoId === null)
            throw new Error("The parameter 'toDoId' must be defined.");
        url_ = url_.replace("{toDoId}", encodeURIComponent("" + toDoId));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processToDoGETGET(_response);
        });
    }

    protected processToDoGETGET(response: Response): Promise<ToDo> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = ToDo.fromJS(resultData200);
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<ToDo>(null as any);
    }

    /**
     * @return Success
     */
    toDoDELETEDELETE(projectId: string, todoId: string): Promise<void> {
        let url_ = this.baseUrl + "/api/ToDo/{projectId}/{todoId}";
        if (projectId === undefined || projectId === null)
            throw new Error("The parameter 'projectId' must be defined.");
        url_ = url_.replace("{projectId}", encodeURIComponent("" + projectId));
        if (todoId === undefined || todoId === null)
            throw new Error("The parameter 'todoId' must be defined.");
        url_ = url_.replace("{todoId}", encodeURIComponent("" + todoId));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "DELETE",
            headers: {
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processToDoDELETEDELETE(_response);
        });
    }

    protected processToDoDELETEDELETE(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    /**
     * @param body (optional) 
     * @return Success
     */
    toDoPUTPUT(projectId: string, toDoId: string, body: ToDo | undefined): Promise<void> {
        let url_ = this.baseUrl + "/api/ToDo/{projectId}/{toDoId}";
        if (projectId === undefined || projectId === null)
            throw new Error("The parameter 'projectId' must be defined.");
        url_ = url_.replace("{projectId}", encodeURIComponent("" + projectId));
        if (toDoId === undefined || toDoId === null)
            throw new Error("The parameter 'toDoId' must be defined.");
        url_ = url_.replace("{toDoId}", encodeURIComponent("" + toDoId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processToDoPUTPUT(_response);
        });
    }

    protected processToDoPUTPUT(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    /**
     * @return Success
     */
    userAllAll(): Promise<User[]> {
        let url_ = this.baseUrl + "/api/User";
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processUserAllAll(_response);
        });
    }

    protected processUserAllAll(response: Response): Promise<User[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(User.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<User[]>(null as any);
    }

    /**
     * @param body (optional) 
     * @return Success
     */
    userPOSTPOST(body: User | undefined): Promise<void> {
        let url_ = this.baseUrl + "/api/User";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processUserPOSTPOST(_response);
        });
    }

    protected processUserPOSTPOST(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    /**
     * @return Success
     */
    userGETGET(userId: string): Promise<User> {
        let url_ = this.baseUrl + "/api/User/{userId}";
        if (userId === undefined || userId === null)
            throw new Error("The parameter 'userId' must be defined.");
        url_ = url_.replace("{userId}", encodeURIComponent("" + userId));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "GET",
            headers: {
                "Accept": "text/plain"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processUserGETGET(_response);
        });
    }

    protected processUserGETGET(response: Response): Promise<User> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = User.fromJS(resultData200);
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<User>(null as any);
    }

    /**
     * @param body (optional) 
     * @return Success
     */
    userPUTPUT(userId: string, body: User | undefined): Promise<void> {
        let url_ = this.baseUrl + "/api/User/{userId}";
        if (userId === undefined || userId === null)
            throw new Error("The parameter 'userId' must be defined.");
        url_ = url_.replace("{userId}", encodeURIComponent("" + userId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(body);

        let options_: RequestInit = {
            body: content_,
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processUserPUTPUT(_response);
        });
    }

    protected processUserPUTPUT(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }

    /**
     * @return Success
     */
    userDELETEDELETE(userId: string): Promise<void> {
        let url_ = this.baseUrl + "/api/User/{userId}";
        if (userId === undefined || userId === null)
            throw new Error("The parameter 'userId' must be defined.");
        url_ = url_.replace("{userId}", encodeURIComponent("" + userId));
        url_ = url_.replace(/[?&]$/, "");

        let options_: RequestInit = {
            method: "DELETE",
            headers: {
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processUserDELETEDELETE(_response);
        });
    }

    protected processUserDELETEDELETE(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(null as any);
    }
}

export class Project implements IProject {
    projectId?: string;
    title?: string | undefined;
    operationId?: string;
    createdAt?: Date;
    createdBy?: string | undefined;
    modifiedAt?: Date;
    modifiedBy?: string | undefined;
    serialVersion?: number;

    constructor(data?: IProject) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.projectId = _data["projectId"];
            this.title = _data["title"];
            this.operationId = _data["operationId"];
            this.createdAt = _data["createdAt"] ? new Date(_data["createdAt"].toString()) : <any>undefined;
            this.createdBy = _data["createdBy"];
            this.modifiedAt = _data["modifiedAt"] ? new Date(_data["modifiedAt"].toString()) : <any>undefined;
            this.modifiedBy = _data["modifiedBy"];
            this.serialVersion = _data["serialVersion"];
        }
    }

    static fromJS(data: any): Project {
        data = typeof data === 'object' ? data : {};
        let result = new Project();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["projectId"] = this.projectId;
        data["title"] = this.title;
        data["operationId"] = this.operationId;
        data["createdAt"] = this.createdAt ? this.createdAt.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["modifiedAt"] = this.modifiedAt ? this.modifiedAt.toISOString() : <any>undefined;
        data["modifiedBy"] = this.modifiedBy;
        data["serialVersion"] = this.serialVersion;
        return data;
    }
}

export interface IProject {
    projectId?: string;
    title?: string | undefined;
    operationId?: string;
    createdAt?: Date;
    createdBy?: string | undefined;
    modifiedAt?: Date;
    modifiedBy?: string | undefined;
    serialVersion?: number;
}

export class ToDo implements IToDo {
    toDoId?: string;
    projectId?: string;
    userId?: string;
    title?: string | undefined;
    done?: boolean;
    operationId?: string;
    createdAt?: Date;
    createdBy?: string | undefined;
    modifiedAt?: Date;
    modifiedBy?: string | undefined;
    serialVersion?: number;

    constructor(data?: IToDo) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.toDoId = _data["toDoId"];
            this.projectId = _data["projectId"];
            this.userId = _data["userId"];
            this.title = _data["title"];
            this.done = _data["done"];
            this.operationId = _data["operationId"];
            this.createdAt = _data["createdAt"] ? new Date(_data["createdAt"].toString()) : <any>undefined;
            this.createdBy = _data["createdBy"];
            this.modifiedAt = _data["modifiedAt"] ? new Date(_data["modifiedAt"].toString()) : <any>undefined;
            this.modifiedBy = _data["modifiedBy"];
            this.serialVersion = _data["serialVersion"];
        }
    }

    static fromJS(data: any): ToDo {
        data = typeof data === 'object' ? data : {};
        let result = new ToDo();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["toDoId"] = this.toDoId;
        data["projectId"] = this.projectId;
        data["userId"] = this.userId;
        data["title"] = this.title;
        data["done"] = this.done;
        data["operationId"] = this.operationId;
        data["createdAt"] = this.createdAt ? this.createdAt.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["modifiedAt"] = this.modifiedAt ? this.modifiedAt.toISOString() : <any>undefined;
        data["modifiedBy"] = this.modifiedBy;
        data["serialVersion"] = this.serialVersion;
        return data;
    }
}

export interface IToDo {
    toDoId?: string;
    projectId?: string;
    userId?: string;
    title?: string | undefined;
    done?: boolean;
    operationId?: string;
    createdAt?: Date;
    createdBy?: string | undefined;
    modifiedAt?: Date;
    modifiedBy?: string | undefined;
    serialVersion?: number;
}

export class User implements IUser {
    userId?: string;
    userName?: string | undefined;
    operationId?: string;
    createdAt?: Date;
    createdBy?: string | undefined;
    modifiedAt?: Date;
    modifiedBy?: string | undefined;
    serialVersion?: number;

    constructor(data?: IUser) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.userId = _data["userId"];
            this.userName = _data["userName"];
            this.operationId = _data["operationId"];
            this.createdAt = _data["createdAt"] ? new Date(_data["createdAt"].toString()) : <any>undefined;
            this.createdBy = _data["createdBy"];
            this.modifiedAt = _data["modifiedAt"] ? new Date(_data["modifiedAt"].toString()) : <any>undefined;
            this.modifiedBy = _data["modifiedBy"];
            this.serialVersion = _data["serialVersion"];
        }
    }

    static fromJS(data: any): User {
        data = typeof data === 'object' ? data : {};
        let result = new User();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userId"] = this.userId;
        data["userName"] = this.userName;
        data["operationId"] = this.operationId;
        data["createdAt"] = this.createdAt ? this.createdAt.toISOString() : <any>undefined;
        data["createdBy"] = this.createdBy;
        data["modifiedAt"] = this.modifiedAt ? this.modifiedAt.toISOString() : <any>undefined;
        data["modifiedBy"] = this.modifiedBy;
        data["serialVersion"] = this.serialVersion;
        return data;
    }
}

export interface IUser {
    userId?: string;
    userName?: string | undefined;
    operationId?: string;
    createdAt?: Date;
    createdBy?: string | undefined;
    modifiedAt?: Date;
    modifiedBy?: string | undefined;
    serialVersion?: number;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}