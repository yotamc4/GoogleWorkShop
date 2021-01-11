import * as React from "react";
import * as FormsStyles from "../FormStyles/FormsStyles";
import { Stack, Dropdown } from "office-ui-fabric-react";
import {
  DatePicker,
  DayOfWeek,
  DefaultButton,
  IDatePickerStrings,
  IDropdownOption,
  PrimaryButton,
  Separator,
  Text,
  TextField,
  Image,
  MessageBar,
  MessageBarType,
  Spinner,
} from "@fluentui/react";
import { CategoriesMap } from "../HomePage/Model/Categories";
import { newProductRequest } from "../Modal/ProductDetails";
import { NewBidRequest } from "../Modal/GroupDetails";
import { submitNewGroupForm } from "../Services/BidsControllerService";
import { useHistory } from "react-router";
import ButtonAppBar from "../LoginBar";
import { useAuth0 } from "@auth0/auth0-react";
import { stringNotContainsOnlyNumbers } from "../Utils/FormUtils";

export const NewGroupBuyingForm: React.FunctionComponent = () => {
  const [productDetails, setProductDetails] = React.useReducer<
    (
      prevState: Partial<newProductRequest>,
      state: Partial<newProductRequest>
    ) => newProductRequest
  >(
    (prevState: any, state: any) => ({
      ...prevState,
      ...state,
    }),
    {
      name: "",
      image: undefined,
      description: "",
    }
  );

  const [bidRequest, setBidRequest] = React.useReducer<
    (
      prevState: Partial<NewBidRequest>,
      state: Partial<NewBidRequest>
    ) => NewBidRequest
  >(
    (prevState: any, state: any) => ({
      ...prevState,
      ...state,
    }),
    {
      ownerId: "1",
      category: "",
      subCategory: "",
      // Tomorrow Date
      expirationDate: new Date(new Date().setDate(new Date().getDate() + 1)),
      maxPrice: 0,
      product: undefined,
    }
  );

  const [
    allRequiredFieldsAreFulfilled,
    SetAllRequiredFieldsAreFulfilled,
  ] = React.useState<boolean>(false);

  const [inputsAreValid, setInputsAreValid] = React.useState<boolean>(true);

  const [requestInProcess, setRequestInProcess] = React.useState<boolean>(
    false
  );

  const [errorMessage, setErrorMessage] = React.useState<string>();
  const urlHistory = useHistory();

  React.useEffect(() => {
    const allRequiredFieldsAreFulfilledObjectTemp: boolean =
      Object.values(bidRequest).every((el) => el) &&
      !!productDetails.description &&
      !!productDetails.name;

    SetAllRequiredFieldsAreFulfilled(allRequiredFieldsAreFulfilledObjectTemp);
  }, [bidRequest, productDetails]);

  React.useEffect(() => {
    setBidRequest({ product: productDetails });
  }, [productDetails]);

  function onDropdownChange(
    event: React.FormEvent<HTMLDivElement>,
    option?: IDropdownOption
  ): void {
    const id: string | undefined = option?.id as string;
    const text: string | undefined = option?.text!;
    setBidRequest({ [id]: text });
  }

  const onTextFieldChange = (
    setStateFunction: Function,
    event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>,
    newValue?: string,
    convertToNumber?: boolean,
    lengthLimit?: number
  ): void => {
    if (lengthLimit && newValue && newValue?.length > lengthLimit) {
      newValue = "";
    }

    const updatedValue: string | number | undefined = convertToNumber
      ? Number(newValue)
      : newValue;

    setStateFunction({
      [(event.target as HTMLInputElement).id]: updatedValue,
    });
  };

  const { getAccessTokenSilently } = useAuth0();

  const onSubmitForm = async (): Promise<void> => {
    try {
      setRequestInProcess(true);
      bidRequest.expirationDate.setHours(23, 59, 0, 0);
      await submitNewGroupForm(bidRequest, getAccessTokenSilently);
      urlHistory.push(`/`);
    } catch {
      setRequestInProcess(false);
      setErrorMessage(
        "An error occurred while trying to create your group. Please try again later."
      );
    }
  };

  return (
    <Stack horizontalAlign={"center"}>
      <ButtonAppBar />
      <Text
        block={true}
        className="Bold"
        styles={FormsStyles.headerStyle}
        variant="xLargePlus"
      >
        Create new group-buy
      </Text>
      <Stack
        styles={{ root: { width: "40%" } }}
        tokens={FormsStyles.verticalGapStackTokens}
      >
        <Separator styles={{ root: { width: "100%" } }} />
        {errorMessage && (
          <MessageBar
            messageBarType={MessageBarType.error}
            onDismiss={() => setErrorMessage("")}
          >
            {errorMessage}
          </MessageBar>
        )}
        <TextField
          id="name"
          label="Product's name"
          styles={{ root: { width: FormsStyles.inputWidth } }}
          onChange={(event, newValue) => {
            onTextFieldChange(setProductDetails, event, newValue, false, 70);
          }}
          onGetErrorMessage={(value) => {
            if (value.length > 70) {
              setInputsAreValid(false);
              return "Product's name can not be longer than 70 chars";
            } else {
              setInputsAreValid(true);
              return "";
            }
          }}
          required
        />
        <Dropdown
          id="category"
          onChange={onDropdownChange}
          label={"Product's Category"}
          styles={FormsStyles.dropdownStyles}
          options={Array.from(CategoriesMap.keys()).map((categoryName) => ({
            key: categoryName,
            id: "category",
            text: categoryName,
          }))}
          required
        ></Dropdown>
        <Dropdown
          id="subCategory"
          label={"Product's Sub Category"}
          onChange={onDropdownChange}
          styles={FormsStyles.dropdownStyles}
          options={
            bidRequest.category
              ? Array.from(
                  CategoriesMap.get(bidRequest.category)!.map(
                    (subCategory) => ({
                      key: subCategory,
                      id: "subCategory",
                      text: subCategory,
                    })
                  )
                )
              : []
          }
          required
        ></Dropdown>
        <TextField
          id="maxPrice"
          onChange={(event, newValue) => {
            if (stringNotContainsOnlyNumbers(newValue)) {
              newValue = "";
            }

            onTextFieldChange(setBidRequest, event, newValue);
          }}
          label="Maximum price"
          onGetErrorMessage={(value) => {
            if (stringNotContainsOnlyNumbers(value)) {
              setInputsAreValid(false);
              return "Only numbers allowed";
            } else {
              setInputsAreValid(true);
              return "";
            }
          }}
          styles={{ root: { width: FormsStyles.inputWidth } }}
          suffix="₪"
          required
        />
        <DatePicker
          id="expirationDate"
          onSelectDate={(date: Date | null | undefined): void => {
            setBidRequest({
              expirationDate: date!,
            });
          }}
          value={bidRequest.expirationDate ?? undefined}
          label="Set a last day to join"
          firstDayOfWeek={DayOfWeek.Sunday}
          strings={DayPickerStrings}
          placeholder="Select a date"
          styles={{ root: { width: FormsStyles.inputWidth } }}
          isRequired
          // Tomorrow Date
          minDate={new Date(new Date().setDate(new Date().getDate() + 1))}
        />
        <TextField
          id="description"
          onChange={(event, newValue) => {
            onTextFieldChange(setProductDetails, event, newValue, false, 1000);
          }}
          label="Description"
          multiline
          autoAdjustHeight
          styles={{ root: { width: FormsStyles.inputWidth } }}
          onGetErrorMessage={(value) => {
            if (value.length > 6000) {
              setInputsAreValid(false);
              return "Description can not be longer than 6000 chars";
            } else {
              setInputsAreValid(true);
              return "";
            }
          }}
          required
        />
        <TextField
          id="image"
          label="Enter URL image for reference"
          styles={{ root: { width: FormsStyles.inputWidth } }}
          onChange={(event, newValue) => {
            onTextFieldChange(setProductDetails, event, newValue);
          }}
        />
        {productDetails?.image && (
          <Image
            src={productDetails.image}
            id="target"
            width={"25rem"}
            height={"20rem"}
          />
        )}
        <Separator styles={{ root: { width: FormsStyles.inputWidth } }} />
        <Stack horizontal horizontalAlign={"space-between"}>
          <DefaultButton text="Cancel" href={"/"} />
          {requestInProcess && <Spinner />}
          <PrimaryButton
            text="Create"
            onClick={() => {
              onSubmitForm();
            }}
            disabled={!allRequiredFieldsAreFulfilled || !inputsAreValid}
          />
        </Stack>
      </Stack>
    </Stack>
  );
};

const DayPickerStrings: IDatePickerStrings = {
  months: [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ],

  shortMonths: [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec",
  ],

  days: [
    "Sunday",
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday",
  ],

  shortDays: ["S", "M", "T", "W", "T", "F", "S"],

  goToToday: "Go to today",
  prevMonthAriaLabel: "Go to previous month",
  nextMonthAriaLabel: "Go to next month",
  prevYearAriaLabel: "Go to previous year",
  nextYearAriaLabel: "Go to next year",
  closeButtonAriaLabel: "Close date picker",
  monthPickerHeaderAriaLabel: "{0}, select to change the year",
  yearPickerHeaderAriaLabel: "{0}, select to change the month",
};
