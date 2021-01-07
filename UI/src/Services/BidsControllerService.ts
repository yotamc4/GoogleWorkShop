import { Bid, NewBidRequest } from "../Modal/GroupDetails";
import { BidBuyerJoinRequest } from "../Modal/ProductDetails";
import { ISupplierProposalRequest } from "../ProductPage/Suppliers/SupplierSection.interface";
import {
  buildUrlWithQueryParams,
  makeDeleteRequest,
  makePostRequest,
} from "./ControllerUtils";

export async function submitNewGroupForm(
  bidRequest: NewBidRequest,
  getAccessTokenSilently?: (options?: any) => Promise<string>
): Promise<void> {
  const serviceUrl = BasicControllerUrl;

  await makePostRequest(serviceUrl, bidRequest, getAccessTokenSilently);
}

export async function getBids(
  intervalNumber: number,
  category: string | null,
  subCategory: string | null,
  searchString: string | null
): Promise<GetBidsResponse> {
  const serviceUrl = BasicControllerUrl;
  const queryParm: Map<string, string> = new Map([
    ["page", intervalNumber.toString()],
  ]);

  if (category) {
    queryParm.set("category", category);
  }

  if (subCategory) {
    queryParm.set("subCategory", subCategory);
  }

  if (searchString) {
    queryParm.set("search", searchString);
  }

  const options = {
    method: "GET",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json;",
    },
  };

  try {
    const response: Response = await fetch(
      buildUrlWithQueryParams(serviceUrl, queryParm),
      options
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

export function addBuyer(
  bidBuyerJoinRequest: BidBuyerJoinRequest,
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  const response = makePostRequest(
    serviceUrl,
    bidBuyerJoinRequest,
    getAccessTokenSilently
  );
  return response;
}

export function deleteBuyer(
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  const response = makeDeleteRequest(serviceUrl, "", getAccessTokenSilently);
  return response;
}

export function addSupplierProposal(
  supplierProposalFormDetails: Partial<ISupplierProposalRequest>,
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  const response = makePostRequest(
    serviceUrl,
    supplierProposalFormDetails,
    getAccessTokenSilently
  );
  return response;
}

export function deleteSupplierProposal(
  url: string,
  getAccessTokenSilently?: (options?: any) => Promise<string>
) {
  const serviceUrl = BasicControllerUrl + url;
  const response = makeDeleteRequest(serviceUrl, "", getAccessTokenSilently);
  return response;
}

const BasicControllerUrl: string = "https://localhost:5001/api/v1/Bids";

export interface GetBidsResponse {
  pageSize: number;
  numberOfPages: number;
  pageNumber: number;
  bidsPage: Bid[];
}
