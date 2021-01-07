export async function makePostRequest<T>(
  url: string,
  body: T,
  getAccessTokenSilently?: (options?: any) => Promise<string>
): Promise<Response> {
  const options: any = {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json;",
    },
    body: JSON.stringify(body),
  };

  if (getAccessTokenSilently) {
    options.headers.Authorization = `Bearer ${await getAccessTokenSilently()}`;
  }

  return await fetch(url, options);
}

export async function makeDeleteRequest<T>(
  url: string,
  body: T,
  getAccessTokenSilently?: (options?: any) => Promise<string>
): Promise<Response> {
  const options: any = {
    method: "DELETE",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json;",
    },
    body: JSON.stringify(body),
  };

  if (getAccessTokenSilently) {
    options.headers.Authorization = `Bearer ${await getAccessTokenSilently()}`;
  }

  return await fetch(url, options);
}

export async function makeGetRequest<T>(
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
): Promise<Response> {
  const options: any = {
    method: "GET",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json;",
    },
  };

  if (getAccessTokenSilently) {
    options.headers.Authorization = `Bearer ${await getAccessTokenSilently()}`;
  }

  return await fetch(url, options);
}

export function buildUrlWithQueryParams(
  stringUrl: string,
  params: Map<string, string>
): string {
  const url: URL = new URL(stringUrl);
  params.forEach((value, key) => url.searchParams.append(key, value));

  return url.toString();
}

export interface queryParamsObjectType {
  [queryName: string]: string;
}
