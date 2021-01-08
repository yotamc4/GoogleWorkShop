import * as ControllerUtils from "./ControllerUtils";
import { GetBidsResponse } from "./BidsControllerService";

export async function GetGroupsSupplierHasWon(
  getAccessTokenSilently?: (options?: any) => Promise<string>
): Promise<GetBidsResponse> {
  const serviceUrl = ControllerUtils.buildUrlWithQueryParams(
    BasicControllerUrl,
    new Map([["IsChosenSupplier", "true"]])
  );

  try {
    const response: Response = await ControllerUtils.makeGetRequest(
      serviceUrl,
      getAccessTokenSilently
    );

    const getBidsResponse: GetBidsResponse = (await response.json()) as GetBidsResponse;

    for (let i = 0; i < getBidsResponse.bidsPage.length; i++) {
      getBidsResponse.bidsPage[i].creationDate = new Date(
        getBidsResponse.bidsPage[i].creationDate
      );

      getBidsResponse.bidsPage[i].expirationDate = new Date(
        getBidsResponse.bidsPage[i].expirationDate!
      );
    }

    return getBidsResponse;
  } catch (e) {
    throw e;
  }
}

export async function GetGroupsSupplierIsParticipant(
  getAccessTokenSilently?: (options?: any) => Promise<string>
): Promise<GetBidsResponse> {
  const serviceUrl = BasicControllerUrl;

  try {
    const response: Response = await ControllerUtils.makeGetRequest(
      serviceUrl,
      getAccessTokenSilently
    );

    const getBidsResponse: GetBidsResponse = (await response.json()) as GetBidsResponse;

    for (let i = 0; i < getBidsResponse.bidsPage.length; i++) {
      getBidsResponse.bidsPage[i].creationDate = new Date(
        getBidsResponse.bidsPage[i].creationDate
      );

      getBidsResponse.bidsPage[i].expirationDate = new Date(
        getBidsResponse.bidsPage[i].expirationDate!
      );
    }

    return getBidsResponse;
  } catch (e) {
    throw e;
  }
}

const BasicControllerUrl: string =
  "https://localhost:5001/api/v1/Suppliers/bids";
