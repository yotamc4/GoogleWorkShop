import * as ControllerUtils from "./ControllerUtils";

export async function getAutoCompleteValues(): Promise<string[]> {
  try {
    const response: Response = await ControllerUtils.makeGetRequest(
      BasicControllerUrl + "/Products"
    );

    const autoCompleteValues: any = (await response.json()) as any;

    return autoCompleteValues as string[];
  } catch (e) {
    throw e;
  }
}

const BasicControllerUrl: string = "https://localhost:5001/api/v1/Autocomplete";
