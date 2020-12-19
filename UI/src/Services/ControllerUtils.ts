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

export function buildUrlWithQueryParams(
  stringUrl: string,
  params: queryParamsObjectType
): string {
  const url: URL = new URL(stringUrl);
  Object.keys(params).forEach((key) =>
    url.searchParams.append(key, params[key])
  );

  return url.toString();
}

export interface queryParamsObjectType {
  [queryName: string]: string;
}
