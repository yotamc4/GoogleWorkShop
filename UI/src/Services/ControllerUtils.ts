export async function makePostRequest<T>(
  url: string,
  body: T
): Promise<Response> {
  const options = {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json;",
    },
    body: JSON.stringify(body),
  };

  return await fetch(url, options);
}

export async function makeGetRequest<T>(url: string): Promise<Response> {
  const options = {
    method: "GET",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json;",
    },
  };

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
